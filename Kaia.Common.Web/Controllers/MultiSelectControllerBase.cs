using Kaia.Common.DataAccess;
using Kaia.Common.DataAccess.Contract;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace Kaia.Common.Web.Controllers
{
    /// <summary>
    /// Base controller for multi-record editing
    /// </summary>
    public abstract class MultiSelectControllerBase<TCreateModel, TUpdateModel> : 
        ControllerBase
        where TCreateModel : class, new()
        where TUpdateModel : class, IEntityModifier
    {
        public MultiSelectControllerBase() : base() { }


        /// <summary>
        /// Code to execute on creation of new entity
        /// </summary>
        protected abstract void DoCreate(TCreateModel model);


        /// <summary>
        /// Code to execute on creation of existing entity (edit, duplicate, 
        /// delete, etc.)
        /// </summary>
        protected abstract void DoUpdate(string taskName, TUpdateModel model);


        /// <summary>
        /// Override this if there are additional task types that need handling
        /// </summary>
        protected virtual ActionResult HandleNonStandardTask(string taskName)
        {
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Code to be implemented that creates the EntityModifier instance to 
        /// represent the modification operation
        /// </summary>
        /// <param name="ids">The IDs of the entities to be modified</param>
        protected abstract TUpdateModel GetEntityModifier(IEnumerable<long> ids);


        protected virtual ActionResult RouteTask(string taskName, long[] ids = null)
        {
            if (taskName == CommonTasks.New)
            {
                return RedirectToAction("Create");
            }
            else
            {
                if (ids != null && ids.Length > 0)
                {
                    var entityModifier = GetEntityModifier(ids);
                    ActionResult resultingAction = RedirectToAction(taskName);
                    if (taskName == CommonTasks.Edit)
                    {
                        entityModifier.ModificationType = ModificationType.Update;
                    }
                    else if (taskName == CommonTasks.Duplicate)
                    {
                        entityModifier.ModificationType = ModificationType.Duplicate;
                    }
                    else if (taskName == CommonTasks.Delete)
                    {
                        entityModifier.ModificationType = ModificationType.Delete;
                    }
                    else
                    {
                        resultingAction = HandleNonStandardTask(taskName);
                    }
                    // Pass in TempData rather than serialising to a URL
                    TempData["Kaia.Model"] = entityModifier;
                    return resultingAction;
                }
            }
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Returns the standard action for creating a new entity
        /// </summary>
        protected virtual ActionResult GetCreate()
        {
            var model = new TCreateModel();
            return ViewOrPartial(ConfigureModel(model));
        }


        /// <summary>
        /// Returns the standard action for modifying existing entities
        /// </summary>
        protected virtual ActionResult GetModification(string viewName = "Edit")
        {
            var model = GetModelFromTempData();
            // If the user tries to navigate to an edit page without going 
            // via the index page, drop them back to the index page with a 
            // warning message
            if (model == null)
            {
                SetMessageText("Please select items to modify");
                return RedirectToAction("Index");
            }
            return ViewOrPartial(viewName, ConfigureModel(model));
        }


        protected virtual ActionResult PostModification(string taskName, 
            object model, string viewName = "Edit")
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            if (ModelState.IsValid)
            {
                try
                {
                    if (taskName == CommonTasks.New)
                    {
                        DoCreate((TCreateModel)model);
                    }
                    else
                    {
                        DoUpdate(taskName, (TUpdateModel)model);
                    }
                    if (Request.IsAjaxRequest())
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.OK);
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception exc)
                {
                    Logger.Error(exc);
                    ModelState.AddModelError("", exc);
                    Response.StatusCode = 
                        (int)HttpStatusCode.InternalServerError;
                }
            }
            if (taskName == CommonTasks.New)
            {
                return ViewOrPartial(viewName, 
                    ConfigureModel((TCreateModel)model));
            }
            return ViewOrPartial(viewName, 
                ConfigureModel((TUpdateModel)model));
        }


        protected virtual TUpdateModel GetModelFromTempData()
        {
            return TempData["Kaia.Model"] as TUpdateModel;

        }

        /// <summary>
        /// Function to prepare model for display (e.g., populate default 
        /// values, dropdown lists, etc.)
        /// </summary>
        protected virtual TCreateModel ConfigureModel(TCreateModel model)
        {
            return model;
        }

        /// <summary>
        /// Function to prepare model for display (e.g., populate default 
        /// values, dropdown lists, etc.)
        /// </summary>
        protected virtual TUpdateModel ConfigureModel(TUpdateModel model)
        {
            return model;
        }
    }
}
