//using Application.ResponseMessage;
//using Domain.Models.Response;
//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;
//using System.Net;

//namespace API.Controllers
//{
//    [Route("api/v1/tags")]
//    [ApiController]
//    public class TagController : ControllerBase
//    {
//        [HttpGet("all")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<APIResponse> GetAllTag([FromQuery, Range(1, int.MaxValue)] int page = 1,
//                                                                  [FromQuery, Range(1, int.MaxValue)] int eachPage = 10)
//        {
//            APIResponse response = new APIResponse();
//            var result = await _tagService.GetAllTag(page, eachPage);


//            if (result.Any())
//            {
//                response.StatusResponse = HttpStatusCode.OK;
//                response.Message = MessageCommon.Complete;
//                response.Data = result;
//            }
//            else
//            {
//                response.StatusResponse = HttpStatusCode.BadRequest;
//                response.Message = MessageCommon.NotFound;
//            }
//            return response;

//        }

//        [HttpPost("")]
//        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
//        public async Task<APIResponse> AddTag([FromBody] TagDto tagDTO)
//        {
//            APIResponse response = new APIResponse();
//            var result = await _tagService.AddTag(tagDTO);

//            if (result != null)
//            {
//                response.StatusResponse = HttpStatusCode.Created;
//                response.Message = MessageCommon.CreateSuccesfully;
//                response.Data = result;

//            }
//            else
//            {
//                response.StatusResponse = HttpStatusCode.BadRequest;
//                response.Message = MessageCommon.CreateFailed;
//            }
//            return response;

//        }

//        //[HttpPut("")]
//        //[ProducesResponseType(StatusCodes.Status204NoContent)]
//        //[ProducesResponseType(StatusCodes.Status404NotFound)]
//        //public async Task<APIResponse> UpdateTag([FromBody] TagDto tagDTO)
//        //{
//        //    APIResponse response = new APIResponse();
//        //    var result = await _tagService.UpdateTag(tagDTO);
//        //    if (result)
//        //    {
//        //        response.StatusResponse = HttpStatusCode.OK;
//        //        response.Message = MessageCommon.UpdateSuccesfully;
//        //        response.Data = result;

//        //    }
//        //    else
//        //    {
//        //        response.StatusResponse = HttpStatusCode.BadRequest;
//        //        response.Message = MessageCommon.UpdateFailed;
//        //    }
//        //    return response;
//        //}

//        [HttpDelete("")]
//        public async Task<APIResponse> DeleteTag(int TagId)
//        {
//            APIResponse response = new APIResponse();
//            var result = await _tagService.DeleteTag(TagId);
//            if (result)
//            {
//                response.StatusResponse = HttpStatusCode.OK;
//                response.Message = MessageCommon.DeleteSuccessfully;
//                response.Data = result;

//            }
//            else
//            {
//                response.StatusResponse = HttpStatusCode.BadRequest;
//                response.Message = MessageCommon.DeleteFailed;
//            }
//            return response;
//        }


//        [HttpGet("keyword")]
//        public async Task<APIResponse> SearchByKeyWord(string searchTerm)
//        {
//            APIResponse response = new APIResponse();
//            var result = await _tagService.SearchTag(searchTerm);
//            if (result.Count() > 0)
//            {
//                response.StatusResponse = HttpStatusCode.OK;
//                response.Message = MessageCommon.ReturnListHasValue;
//                response.Data = result;

//            }
//            else
//            {
//                response.StatusResponse = HttpStatusCode.BadRequest;
//                response.Message = MessageCommon.ReturnNullList;
//            }
//            return response;
//        }
//        [HttpGet("")]
//        public async Task<IActionResult> GetTagById([FromQuery, Required] int tagId)
//        {
//            var tag = await _tagService.GetById(tagId);
//            if (tag != null)
//            {
//                return Ok(new APIResponse
//                {
//                    StatusResponse = HttpStatusCode.OK,
//                    Message = MessageCommon.Complete,
//                    Data = tag
//                });
//            }
//            return Ok(new APIResponse
//            {
//                StatusResponse = HttpStatusCode.OK,
//                Message = MessageCommon.Complete,
//                Data = null
//            });
//        }
//        [HttpGet("trending")]
//        public async Task<IActionResult> TrendingTask()
//        {
//            var result = await _tagService.TrendingsTags();
//            return Ok(new APIResponse
//            {
//                StatusResponse = HttpStatusCode.OK,
//                Message = MessageCommon.Complete,
//                Data = result
//            });
//        }
//    }
//}
