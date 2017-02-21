namespace OnlineShop.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileTypeAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "Only the following file types are allowed: {0}";
        private IEnumerable<string> ValidTypes { get; set; }

        public FileTypeAttribute(string validTypes)
        {
            this.ValidTypes = validTypes.Split(',').Select(s => s.Trim().ToLower());
            this.ErrorMessage = string.Format(DefaultErrorMessage, string.Join(" or ", this.ValidTypes));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;
            if (file != null && !this.ValidTypes.Any(e => file.FileName.EndsWith(e)))
            {
                return new ValidationResult(this.ErrorMessageString);
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "filetype",
                ErrorMessage = this.ErrorMessageString
            };

            rule.ValidationParameters.Add("validtypes", string.Join(",", this.ValidTypes));
            yield return rule;
        }
    }
}