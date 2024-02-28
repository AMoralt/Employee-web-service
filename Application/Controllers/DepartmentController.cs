using Application.Commands.Departments;
using Application.Handlers.Departments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;
/// <summary>
/// Управление отделами.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class DepartmentController : ControllerBase
{
    private readonly IMediator _mediator;
    public DepartmentController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Создает новый отдел.
    /// </summary>
    /// <remarks>
    /// Для создания отдела необходимо указать все обязательные поля.
    ///
    /// Параметры запроса:
    /// - Name - обязательное поле.
    /// - Phone - обязательное поле.
    /// </remarks>
    /// <param name="command">Команда для создания отдела.</param>
    /// <returns>ID созданного отдела.</returns>
    /// <response code="200">Возвращает ID созданного отдела.</response>
    /// <response code="400">Если запрос некорректен.</response>
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (System.Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    /// <summary>
    /// Возвращает список всех отделов с пагинацией.
    /// </summary>
    /// <remarks>
    /// Этот метод возвращает список отделов с возможностью пагинации. Можно указать limit и offset для управления объемом данных.
    ///
    /// Параметры запроса:
    /// - limit - необязательное поле.
    /// - offset - необязательное поле.
    /// </remarks>
    /// <param name="limit">Ограничение количества возвращаемых записей.</param>
    /// <param name="offset">Смещение начала выборки.</param>
    /// <returns>Список отделов.</returns>
    /// <response code="200">Возвращает список отделов.</response>
    /// <response code="400">Если запрос некорректен.</response>
    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] int limit, [FromQuery] int offset, CancellationToken cancellationToken)
    {
        try
        {
            var command = new GetDepartmentsQuery
            {
                Limit = limit,
                Offset = offset
            };

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (System.Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    /// <summary>
    /// Обновляет данные отдела.
    /// </summary>
    /// <remarks>
    /// Этот метод позволяет обновить данные отдела. Необходимо указать ID отдела и данные, которые требуется обновить.
    ///
    /// Все поля кроме ID являются необязательными и будут обновлены только если они предоставлены.
    /// </remarks>
    /// <param name="command">Команда обновления отдела.</param>
    /// <returns>Результат выполнения операции.</returns>
    /// <response code="200">Если обновление прошло успешно.</response>
    /// <response code="400">Если запрос некорректен.</response>
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateDepartmentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mediator.Send(command, cancellationToken);
            return result ? Ok("Update successful") : BadRequest("Update failed");
        }
        catch (System.Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    /// <summary>
    /// Удаляет отдел по ID.
    /// </summary>
    /// <remarks>
    /// Этот метод позволяет удалить отдел из системы по его идентификатору.
    ///
    /// Параметры запроса:
    /// - departmentId - обязательное поле.
    /// </remarks>
    /// <param name="departmentId">ID отдела для удаления.</param>
    /// <returns>Результат выполнения операции.</returns>
    /// <response code="200">Если удаление прошло успешно.</response>
    /// <response code="400">Если запрос некоррект
    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] int departmentId, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteDepartmentCommand
            {
                DepartmentId = departmentId
            };
            
            var result = await _mediator.Send(command, cancellationToken);
            return result ? Ok("Delete successful") : BadRequest("Delete failed");
        }
        catch (System.Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}