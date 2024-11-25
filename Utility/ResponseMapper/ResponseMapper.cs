namespace Utility.ResponseMapper
{
    public class ResponseMapper<T> : IResponse<T> where T : new()
    {
        //blic IEnumerable<T> DataList { get; set; }

        public ResponseMapper()
        {
            this.Title = string.Empty;
            this.Message = string.Empty;
            this.Success = false;
            this.StatusCode = 0;
        }
        public void GetAll(T list)
        {
            this.Title = "Success";
            this.Message = "Get All Success";
            this.Success = true;
            this.Data = list;
            this.StatusCode = 200;
        }

        // Method to handle a collection of objects
        //public void GetAll(IEnumerable<T> list)
        //{
        //    if (list != null && list.Any())
        //    {
        //        this.Title = "Success";
        //        this.Message = "Operation successful";
        //        this.Success = true;
        //        this.DataList = list;
        //        this.StatusCode = 200;
        //    }
        //    else
        //    {
        //        this.Title = "Failed";
        //        this.Message = "No records found";
        //        this.Success = false;
        //        this.StatusCode = 404;
        //    }
        //}
        public void GetDefault(T model)
        {
            if (model is not null)
            {
                this.Title = "Success";
                this.Message = "Get Default Success";
                this.Success = true;
                this.Data = model;
                this.StatusCode = 200;
            }
            else
            {
                this.Title = "Failed";
                this.Message = "No record found";
                this.Success = false;
                this.StatusCode = 400;
            }
        }
        public void GetById(T model)
        {
            if (model is not null)
            {
                this.Title = "Success";
                this.Message = "GetById Success";
                this.Success = true;
                this.Data = model;
                this.StatusCode = 200;
            }
            else
            {
                this.Title = "Failed";
                this.Message = "No record found";
                this.Success = false;
                this.StatusCode = 400;
            }
        }
        public void Create(T model)
        {
            if (model is not null)
            {
                this.Title = "Success";
                this.Message = "Created successfully";
                this.Success = true;
                this.Data = model;
                this.StatusCode = 200;
            }
            else
            {
                this.Title = "Failed";
                this.Message = "Unable to create";
                this.Success = false;
                this.Data = model;
                this.StatusCode = 400;
            }
        }
        public void Update(T model)
        {
            if (model is not null)
            {
                this.Title = "Success";
                this.Message = "Update successfully";
                this.Success = true;
                this.Data = model;
                this.StatusCode = 200;
            }
            else
            {
                this.Title = "Failed";
                this.Message = "Unable to update";
                this.Success = false;
                this.StatusCode = 400;
            }
        }
        public void Update(bool status)
        {
            if (status)
            {
                this.Title = "Success";
                this.Message = "Updated successfully";
                this.Success = status;
                this.StatusCode = 200;
            }
            else
            {
                this.Title = "Failed";
                this.Message = "Unable to update";
                this.Success = status;
                this.StatusCode = 300;
            }
        }
        public void Delete(bool status)
        {
            if (status)
            {
                this.Title = "Success";
                this.Message = "Deleted successfully";
                this.Success = status;
                this.StatusCode = 200;
            }
            else
            {
                this.Title = "Failed";
                this.Message = "Unable to delete";
                this.Success = status;
                this.StatusCode = 400;
            }
        }
        public void Delete(T model)
        {
            if (model is not null)
            {
                this.Title = "Success";
                this.Message = "Deleted successfully";
                this.Success = true;
                this.Data = model;
                this.StatusCode = 200;
            }
            else
            {
                this.Title = "Failed";
                this.Message = "Unable to delete";
                this.Success = false;
                this.Data = model;
                this.StatusCode = 400;
            }
        }
        public void Status(bool status)
        {
            if (status)
            {
                this.Title = "Success";
                this.Message = "Update successfully";
                this.Success = status;
                this.StatusCode = 200;
            }
            else
            {
                this.Title = "Failed";
                this.Message = "Unable to update status";
                this.Success = status;
                this.StatusCode = 400;
            }
        }
        public void NoRecord(T model)
        {
            this.Title = "Success";
            this.Message = "No record found";
            this.Success = false;
            this.Data = model;
            this.StatusCode = 200;
        }
        public void CacheException(Exception ex)
        {
            this.Success = false;
            if (ex.InnerException == null)
            {
                this.Message = ex.Message;
            }
            else
            {
                var msg = (ex.InnerException.Message.ToString().Length > 500 ? ex.InnerException.Message.Substring(1, 500) : ex.InnerException.Message.ToString());
                this.Message = msg.Replace("\"", "");
            }
        }
        public void Error(string error)
        {
            this.Title = "Failed";
            this.Message = error;
            this.Success = false;
            this.StatusCode = 300;
        }
        public void Login(T model)
        {
            if (model is not null)
            {
                this.Title = "Success";
                this.Message = "LoggedIn successfully";
                this.Success = true;
                this.Data = model;
                this.StatusCode = 200;
            }
            else
            {
                this.Title = "Failed";
                this.Message = "login failed";
                this.Success = false;
                this.StatusCode = 400;
            }
        }
        public void ToggleActive(bool status)
        {
            if (status)
            {
                this.Title = "Success";
                this.Message = "Status changed successfully";
                this.Success = status;
                this.StatusCode = 200;
            }
            else
            {
                this.Title = "Success";
                this.Message = "Unable to update Status";
                this.Success = status;
                this.StatusCode = 400;
            }
        }
        public void DisplayReOrder(bool status)
        {
            if (status)
            {
                this.Title = "Success";
                this.Message = "Display order changed successfully";
                this.Success = status;
                this.StatusCode = 200;
            }
            else
            {
                this.Title = "Success";
                this.Message = "Unable to update display order";
                this.Success = status;
                this.StatusCode = 400;
            }
        }

        //public void DisplayOrder(T model)
        //{
        //    this.Title = "Success";
        //    this.Message = "Display order updated successfully";
        //    this.Success = true;
        //    this.Data = model;
        //    this.StatusCode = 200;
        //}
        public void DisplayOrder(bool status)
        {
            this.Title = "Success";
            this.Message = "Display order updated successfully";
            this.Success = status;
            this.StatusCode = 200;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return base.ToString();
        }

    }

}
