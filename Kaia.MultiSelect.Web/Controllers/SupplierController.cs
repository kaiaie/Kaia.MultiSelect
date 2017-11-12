using Kaia.Common.Web;
using Kaia.MultiSelect.DataAccess.Contract;
using Kaia.MultiSelect.Domain;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Kaia.MultiSelect.Web.Controllers
{
    public sealed class SupplierController : 
        Common.Web.Controllers.MultiSelectControllerBase<NewSupplier, SupplierModifier>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierController(IUnitOfWork unitOfWork) : base()
        {
            _unitOfWork = unitOfWork;
        }

        private IUnitOfWork UnitOfWork { get { return _unitOfWork; } }

        [HttpGet]
        public ActionResult Index()
        {
            var model = UnitOfWork.SupplierRepository.GetAll();

            return ViewOrPartial(model);
        }


        [HttpGet]
        public ActionResult Do(string taskName, long[] ids = null)
        {
            return RouteTask(taskName, ids);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return GetCreate();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(NewSupplier model)
        {
            return PostModification(CommonTasks.New, model, "Create");
        }


        [HttpGet]
        public ActionResult Edit()
        {
            return GetModification("Edit");
        }


        [HttpPost]
        public ActionResult Edit(SupplierModifier model)
        {
            return PostModification(CommonTasks.Edit, model, "Edit");
        }


        [HttpGet]
        public ActionResult Duplicate()
        {
            return GetModification("Duplicate");
        }


        [HttpPost]
        public ActionResult Duplicate(SupplierModifier model)
        {
            return PostModification(CommonTasks.Duplicate, model, "Duplicate");
        }


        [HttpGet]
        public ActionResult Delete()
        {
            return GetModification("Delete");
        }


        [HttpPost]
        public ActionResult Delete(SupplierModifier model)
        {
            return PostModification(CommonTasks.Delete, model, "Delete");
        }


        protected override void DoCreate(NewSupplier model)
        {
            UnitOfWork.SupplierRepository.Create(model);
            UnitOfWork.Save();
        }


        protected override void DoUpdate(string taskName, SupplierModifier model)
        {
            UnitOfWork.SupplierRepository.Modify(model);
            UnitOfWork.Save();
        }


        protected override SupplierModifier GetEntityModifier(IEnumerable<long> ids)
        {
            var entities = UnitOfWork.SupplierRepository.Get(ids);
            return entities.GetModifier();
        }
    }
}