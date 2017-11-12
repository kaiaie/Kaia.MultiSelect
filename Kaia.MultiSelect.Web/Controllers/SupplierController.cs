using Kaia.MultiSelect.DataAccess.Contract;
using System.Web.Mvc;

namespace Kaia.MultiSelect.Web.Controllers
{
    public sealed class SupplierController : 
        Common.Web.Controllers.ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierController(IUnitOfWork unitOfWork) : base()
        {
            _unitOfWork = unitOfWork;
        }

        private IUnitOfWork UnitOfWork { get { return _unitOfWork; } }

        // GET: Supplier
        public ActionResult Index()
        {
            var model = UnitOfWork.SupplierRepository.GetAll();

            return ViewOrPartial(model);
        }
    }
}