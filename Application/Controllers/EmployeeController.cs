using Application.Commands.Employees;
using Application.Handlers.Employees;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

/// <summary>
/// Управление сотрудниками.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;
    public EmployeeController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Добавляет нового сотрудника.
    /// </summary>
    /// <remarks>
    /// Для создания сотрудника необходимо ввести все поля.
    ///
    /// Параметры запроса:
    /// - name - обязательное поле.
    /// - surname - обязательное поле.
    /// - phone - обязательное поле, должно соответствовать формату "+[код страны][номер]".
    /// - companyId - обязательное поле, должно быть больше 0.
    /// - passport.type - обязательное поле.
    /// - passport.number - обязательное поле.
    /// - departmentId - обязательное поле, должно быть больше 0.
    /// </remarks>
    /// <param name="command">Команда для создания сотрудника.</param>
    /// <returns>ID добавленного сотрудника.</returns>
    /// <response code="200">Возвращает ID добавленного сотрудника.</response>
    /// <response code="400">Если запрос некорректен.</response>
    [HttpPost]
    public async Task<ActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command, CancellationToken cancellationToken)
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
    /// Возвращает список всех сотрудников с пагинацией.
    /// </summary>
    /// <remarks>
    /// Этот метод возвращает список сотрудников с возможностью пагинации. Можно указать limit и offset для управления объемом данных.
    /// 
    /// Параметры запроса:
    /// - limit - необязательное поле.
    /// - offset - необязательное поле.
    /// </remarks>
    /// <param name="limit">Ограничение количества возвращаемых записей.</param>
    /// <param name="offset">Смещение начала выборки.</param>
    /// <returns>Список сотрудников.</returns>
    /// <response code="200">Возвращает список сотрудников.</response>
    /// <response code="400">Если запрос некорректен.</response>
    [HttpGet]
    public async Task<ActionResult> GetAllEmployee([FromQuery] int limit, [FromQuery] int offset,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new GetEmployeesQuery
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
    /// Возвращает список сотрудников по ID отдела с пагинацией.
    /// </summary>
    /// <remarks>
    /// Этот метод возвращает список сотрудников, принадлежащих определенному отделу, с возможностью пагинации. Можно указать limit и offset для управления объемом данных.
    ///
    /// Параметры запроса:
    /// - departmentId - обязательное поле.
    /// - limit - необязательное поле.
    /// - offset - необязательное поле.
    /// </remarks>
    /// <param name="departmentId">ID отдела.</param>
    /// <param name="limit">Ограничение количества возвращаемых записей.</param>
    /// <param name="offset">Смещение начала выборки.</param>
    /// <returns>Список сотрудников указанного отдела.</returns>
    /// <response code="200">Возвращает список сотрудников отдела.</response>
    /// <response code="400">Если запрос некорректен.</response>
    [HttpGet("[action]")]
    public async Task<ActionResult> GetAllByDepartmentIdEmployee([FromQuery] int departmentId, [FromQuery] int limit, [FromQuery] int offset, CancellationToken cancellationToken)
    {
        try
        {
            var command = new GetEmployeesByDepartmentIdQuery
            {
                DepartmentId = departmentId,
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
    /// Возвращает список сотрудников по ID компании с пагинацией.
    /// </summary>
    /// <remarks>
    /// Этот метод позволяет получить список сотрудников, принадлежащих определенной компании, с возможностью пагинации. Можно указать limit и offset для управления объемом данных.
    /// 
    /// Параметры запроса:
    /// - companyId - обязательное поле.
    /// - limit - необязательное поле.
    /// - offset - необязательное поле.
    /// </remarks>
    /// <param name="companyId">ID компании.</param>
    /// <param name="limit">Ограничение количества возвращаемых записей.</param>
    /// <param name="offset">Смещение начала выборки.</param>
    /// <returns>Список сотрудников указанной компании.</returns>
    /// <response code="200">Возвращает список сотрудников компании.</response>
    /// <response code="400">Если запрос некорректен.</response>
    [HttpGet("[action]")]
    public async Task<ActionResult> GetAllByCompanyIdEmployee([FromQuery] int companyId, [FromQuery] int limit, [FromQuery] int offset, CancellationToken cancellationToken)
    {
        try
        {
            var command = new GetEmployeesByCompanyIdQuery()
            {
                CompanyId = companyId,
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
    /// Обновляет данные сотрудника.
    /// </summary>
    /// <remarks>
    /// Этот метод позволяет обновить данные сотрудника. Необходимо указать ID сотрудника и данные, которые требуется обновить.
    ///
    /// Все поля, кроме ID, являются необязательными и будут обновлены только если они предоставлены.
    /// </remarks>
    /// <param name="command">Команда обновления сотрудника.</param>
    /// <returns>Результат выполнения операции.</returns>
    /// <response code="200">Если обновление прошло успешно.</response>
    /// <response code="400">Если запрос некорректен.</response>
    [HttpPut]
    public async Task<ActionResult> UpdateEmployee([FromBody] UpdateEmployeeCommand command, CancellationToken cancellationToken)
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
    /// Удаляет сотрудника по ID.
    /// </summary>
    /// <remarks>
    /// Этот метод позволяет удалить сотрудника из системы по его идентификатору.
    ///
    /// Параметры запроса:
    /// - departmentId - обязательное поле.
    /// </remarks>
    /// <param name="departmentId">ID сотрудника для удаления.</param>
    /// <returns>Результат выполнения операции.</returns>
    /// <response code="200">Если удаление прошло успешно.</response>
    /// <response code="400">Если запрос некорректен.</response>
    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] int departmentId, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteEmployeeCommand
            {
                Id = departmentId
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