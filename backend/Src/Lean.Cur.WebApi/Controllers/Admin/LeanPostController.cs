using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Admin;

/// <summary>
/// 岗位管理控制器
/// </summary>
[ApiController]
[Route("api/admin/post")]
[Authorize]
public class LeanPostController : LeanBaseController
{
  private readonly ILeanPostService _postService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="postService">岗位服务</param>
  public LeanPostController(ILeanPostService postService)
  {
    _postService = postService;
  }

  /// <summary>
  /// 获取岗位分页列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>岗位分页列表</returns>
  [HttpGet("page")]
  public async Task<LeanApiResponse<PagedResult<LeanPostDto>>> GetPagedListAsync([FromQuery] LeanPostQueryDto queryDto)
  {
    var result = await _postService.GetPagedListAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取岗位详情
  /// </summary>
  /// <param name="id">岗位ID</param>
  /// <returns>岗位详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanApiResponse<LeanPostDto>> GetByIdAsync(long id)
  {
    if (id <= 0)
    {
      return ValidateError<LeanPostDto>("岗位ID必须大于0");
    }
    var result = await _postService.GetByIdAsync(id);
    if (result == null)
    {
      return BusinessError<LeanPostDto>("岗位不存在");
    }
    return Success(result);
  }

  /// <summary>
  /// 创建岗位
  /// </summary>
  /// <param name="createDto">创建参数</param>
  /// <returns>岗位ID</returns>
  [HttpPost]
  public async Task<LeanApiResponse<long>> CreateAsync(LeanPostCreateDto createDto)
  {
    if (string.IsNullOrEmpty(createDto.PostName))
    {
      return ValidateError<long>("岗位名称不能为空");
    }
    if (string.IsNullOrEmpty(createDto.PostCode))
    {
      return ValidateError<long>("岗位编码不能为空");
    }
    var result = await _postService.CreateAsync(createDto);
    return Success(result, "创建岗位成功");
  }

  /// <summary>
  /// 更新岗位
  /// </summary>
  /// <param name="updateDto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut]
  public async Task<LeanApiResponse<bool>> UpdateAsync(LeanPostUpdateDto updateDto)
  {
    if (updateDto.Id <= 0)
    {
      return ValidateError<bool>("岗位ID必须大于0");
    }
    var result = await _postService.UpdateAsync(updateDto);
    return Success(result, result ? "更新岗位成功" : "更新岗位失败");
  }

  /// <summary>
  /// 删除岗位
  /// </summary>
  /// <param name="id">岗位ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<LeanApiResponse<bool>> DeleteAsync(long id)
  {
    if (id <= 0)
    {
      return ValidateError<bool>("岗位ID必须大于0");
    }
    var result = await _postService.DeleteAsync(id);
    return Success(result, result ? "删除岗位成功" : "删除岗位失败");
  }

  /// <summary>
  /// 获取岗位选项列表
  /// </summary>
  /// <returns>岗位选项列表</returns>
  [HttpGet("options")]
  public async Task<LeanApiResponse<List<LeanOptionModel>>> GetOptionsAsync()
  {
    var result = await _postService.GetOptionsAsync();
    return Success(result);
  }
}