using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class AddUserRequest : IDataErrorInfo
    {
        private bool _isValid = true;
        private string _validationMessage;

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public List<long> GroupIds { get; set; }

        public AddUserRequest(string name, List<long> groupIds)
        {
            Name = name;
            GroupIds = groupIds;
            Validate();
        }

        public bool IsValid => _isValid;
        public string ValidationMessage => _validationMessage;
        public string Error => throw new NotImplementedException();
        public string this[string columnName] => throw new NotImplementedException();

        private void Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                _isValid = false;
                _validationMessage = "Name cannot be null or empty";
                return;
            }
        }
    }
}
