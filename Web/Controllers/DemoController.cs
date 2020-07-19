using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.Linq;
using Web.DTO;
using Service;
using Snail.Core.Attributes;

namespace Web.Controllers
{
    /// <summary>
    /// ʾ���ӿ�
    /// </summary>
    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Resource(Description ="����")]
    public class DemoController : DefaultBaseController, ICrudController<Demo, DemoSaveDto, DemoResultDto, DemoQueryDto>
    {
        private DemoService _service;
        public DemoController(DemoService service,ControllerContext controllerContext):base(controllerContext) {
            this.controllerContext = controllerContext;
            this._service = service;
        }

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [Resource(Description ="��ѯ�б�")]
        [HttpGet]
        public List<DemoResultDto> QueryList([FromQuery]DemoQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Demo>();
            return controllerContext.mapper.ProjectTo<DemoResultDto>(_service.QueryList(pred)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [Resource(Description ="��ѯ��ҳ")]
        [HttpGet]
        public IPageResult<DemoResultDto> QueryPage([FromQuery]DemoQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Demo>();
            return controllerContext.mapper.ProjectTo<DemoResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }
        /// <summary>
        /// ���ҵ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Resource(Description ="���ҵ���")]
        [HttpGet]
        public DemoResultDto Find(string id)
        {
            return controllerContext.mapper.Map<DemoResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="ids"></param>
        [Resource(Description ="ɾ��")]
        [HttpPost]
        public void Remove(List<string> ids)
        {
            _service.Remove(ids);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="saveDto"></param>
        [Resource(Description ="����")]
        [HttpPost]
        public void Save(DemoSaveDto saveDto)
        {
            _service.Save(saveDto);
        }
    }
}
