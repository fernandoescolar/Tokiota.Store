namespace Tokiota.Store.Domain
{
    using System;
    using System.Collections.Generic;

    public class CallResult : ICallResult
    {
        private List<string> errors = new List<string>(); 
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get { return this.errors; } }

        public CallResult()
        {
            this.Succeeded = true;
        }
        public CallResult(string error, params string[] errors)
        {
            this.Succeeded = false;
            this.errors.Add(error);
            this.errors.AddRange(errors);
        }

        public void AddErrors(string error)
        {
            this.errors.Add(error);
        }
        public void AddErrors(IEnumerable<string> errormsgs)
        {
            this.errors.AddRange(errormsgs);
        }

        public void AddErrors(Exception ex)
        {
            this.AddErrors(ex.Message);
            if (ex.InnerException != null)
            {
                this.AddErrors(ex.InnerException);
            }
        }

        public static CallResult Merge(ICallResult r1, ICallResult r2)
        {
            var result = new CallResult
                           {
                               Succeeded = r1.Succeeded && r2.Succeeded,
                           };
            result.errors.AddRange(r1.Errors);
            result.errors.AddRange(r2.Errors);
            return result;
        }
    }
}
