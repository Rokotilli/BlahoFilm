namespace BusinessLogicLayer.Models.AdditionalModels
{
    public class ChangeEmailModel
    {
        public string UserEmail
        {
            get => _userEmail;
            set => _userEmail = value.ToLower();
        }
        private string _userEmail;

        public string NewEmail
        {
            get => _newEmail;
            set => _newEmail = value.ToLower();
        }
        private string _newEmail;
    }
}
