using Backend.DTO;
using Backend.Model;
using Backend.Service.Borrows;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class BorrowController : ControllerBase
{
    private readonly IBorrowService _borrowService;

    private readonly EmailService _emailService;

    private readonly Prn232Context _context;
    public BorrowController(IBorrowService borrowService, EmailService emailService , Prn232Context context)
    {
        _borrowService = borrowService;
        _emailService = emailService;
        _context = context;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> CheckoutBooks([FromBody] BorrowRequestDTO request)
    {
        var (success, message) = await _borrowService.CheckoutBooksAsync(request);
        if (success)
            return Ok(new { message });
        return BadRequest(new { message });
    }

    [HttpGet("person/{personId}")]
    public async Task<IActionResult> GetBorrowsByPersonId(int personId)
    {
        var borrows = await _borrowService.GetBorrowsByPersonIdAsync(personId);

        if (borrows == null || !borrows.Any())
        {
            return NotFound(new { message = $"Không tìm thấy phiếu mượn nào cho người dùng có ID = {personId}" });
        }

        return Ok(borrows);
    }

    [HttpGet("detail/{borrowId}")]
    public async Task<IActionResult> GetBorrowDetail(int borrowId)
    {
        var detail = await _borrowService.GetBorrowDetailByBorrowIdAsync(borrowId);
        if (detail == null)
        {
            return NotFound(new { message = $"Không tìm thấy chi tiết phiếu mượn với ID = {borrowId}" });
        }

        return Ok(detail);
    }

    // GET api/borrow/manage?deadlineDate=yyyy-MM-dd
    [HttpGet("manage")]
    public async Task<IActionResult> GetManageBorrows([FromQuery] string? deadlineDate)
    {
        DateOnly? deadline = null;
        if (!string.IsNullOrEmpty(deadlineDate) && DateOnly.TryParse(deadlineDate, out var d))
            deadline = d;
        var result = await _borrowService.GetManageBorrows(deadline);
        return Ok(result);
    }

    [HttpPost("remove-detail")]
    public async Task<IActionResult> RemoveBorrowDetail([FromBody] RemoveBorrowDetailDto req)
    {
        var done = await _borrowService.RemoveBorrowDetail(req.BorrowId, req.BookId);
        if (!done) return NotFound(new { message = "Không tìm thấy chi tiết phiếu mượn cần xóa." });
        return Ok();
    }

    [HttpPost("remove-borrow")]
    public async Task<IActionResult> RemoveBorrow([FromBody] RemoveBorrowDto req)
    {
        var done = await _borrowService.RemoveBorrow(req.BorrowId);
        if (!done) return NotFound(new { message = "Không tìm thấy phiếu mượn cần xóa." });
        return Ok();
    }

    [HttpPost("remove-all")]
    public async Task<IActionResult> RemoveAllBorrows([FromBody] RemoveAllBorrowsDto req)
    {
        var done = await _borrowService.RemoveAllBorrows(req.PersonId);
        if (!done) return NotFound(new { message = "Không tìm thấy borrow với PersonId này." });
        return Ok();
    }

    [HttpPut("update-status")]
    public async Task<IActionResult> UpdateBorrowStatus([FromBody] UpdateBorrowStatusDto dto)
    {
        var result = await _borrowService.UpdateBorrowStatusAsync(dto);
        if (!result)
            return NotFound(new { message = "Không tìm thấy phiếu mượn." });

        return Ok(new { message = "Cập nhật thành công!" });
    }


    [HttpGet("/api/Status")]
    public async Task<IActionResult> GetAllStatuses()
    {
        var statuses = await _borrowService.GetAllStatusesAsync();
        return Ok(statuses);
    }
    [HttpPost("notify-deadline")]
    public async Task<IActionResult> NotifyDeadline([FromBody] NotifyDeadlineRequest request)
    {
        if (!DateOnly.TryParse(request.Deadline, out var deadline))
            return BadRequest(new { message = "Ngày deadline không hợp lệ!" });

        // Lấy những borrow có deadline đúng ngày và còn sách chưa trả (statusId==1)
        var borrows = await _context.Borrows
            .Include(b => b.Person)
            .Include(b => b.BorrowDetails)
            .ThenInclude(d => d.Book) // Nếu có navigation đến Book (để lấy tên sách)
            .Where(b => b.Deadline == deadline && b.BorrowDetails.Any(d => d.StatusId == 1))
            .ToListAsync();
        if (borrows.Count == 0)
        {
            return Ok(new
            {
                message = $"Không có bạn đọc nào cần gửi thông báo hạn trả sách cho ngày {deadline:dd/MM/yyyy}.",
                personIds = new List<int>()
            });
        }

        var sentPersonIds = new List<int>();

        foreach (var b in borrows)
        {
            if (string.IsNullOrEmpty(b.Person?.Email))
                continue;

            // Lấy danh sách chi tiết sách chưa trả
            var bookRows = b.BorrowDetails
                .Where(d => d.StatusId == 1)
                .Select(d => $"<tr><td style='padding:8px 12px;border:1px solid #c8eec6'>{d.Book?.BookName ?? "N/A"}</td>" +
                             $"<td style='padding:8px 12px;border:1px solid #c8eec6;text-align:center'>{d.Amount}</td></tr>")
                .ToList();
            string tableContent = $@"
        <table style='margin:7px 0 10px 0;border-collapse:collapse;font-size:1.06em;min-width:240px;background:#fafef9'>
          <thead>
            <tr>
              <th style='padding:8px 12px;background:#dbf4cd;border:1.4px solid #c8eec6;color:#167a12'>Tên sách</th>
              <th style='padding:8px 12px;background:#dbf4cd;border:1.4px solid #c8eec6;color:#167a12'>Số lượng</th>
            </tr>
          </thead>
          <tbody>
            {string.Join("", bookRows)}
          </tbody>
        </table>";

            string body = $@"
            <div style='font-family:Roboto,sans-serif;background:#f6fff7;padding:18px 12px 8px 12px;border-radius:12px;'>
                <h3 style='margin:0 0 10px 0;color:#26a315;'>THÔNG BÁO HẠN TRẢ SÁCH</h3>
                <div style='margin-bottom:9px;'>
                    <b>Xin chào {b.Person.Name},</b><br>
                    Bạn có đơn mượn sách <b>#{b.BorrowId}</b>, hạn trả: <b style='color:#f28b20'>{b.Deadline:dd/MM/yyyy}</b>.
                </div>
                <div>
                    <span style='font-weight:600;'>Danh sách sách <span style='color:#ff9400'>chưa trả</span>:</span>
                    {tableContent}
                </div>
                <div style='margin:14px 0 9px 0;color:#1d844a;'><b>Vui lòng trả sách đúng hạn để tránh bị phạt!</b></div>
                <div style='font-size:0.97em;color:#169d2c'>Trân trọng,<br>Phòng quản lý thư viện</div>
            </div>";

            try
            {
                string subject = $"Thông báo hạn trả sách (Phiếu mượn #{b.BorrowId})";
                await _emailService.SendAsync(b.Person.Email, subject, body);
                sentPersonIds.Add(b.Person.PersonId);
            }
            catch
            {
                // Log nếu muốn
            }
        }

        return Ok(new
        {
            message = $"Đã gửi email cho {sentPersonIds.Count}/{borrows.Count} bạn đọc còn sách chưa trả, hạn trả {deadline:dd/MM/yyyy}",
            personIds = sentPersonIds
        });
    }



    public class RemoveBorrowDetailDto { public int BorrowId { get; set; } public int BookId { get; set; } }
    public class RemoveBorrowDto { public int BorrowId { get; set; } }
    public class RemoveAllBorrowsDto { public int PersonId { get; set; } }
}
