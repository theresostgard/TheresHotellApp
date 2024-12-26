using HotellApp.Models;
using HotellApp.Models.Enums;

namespace HotellApp.Controllers.GuestInputValidatingControllers
{
    public interface IGuestInputValidatingController
    {
        //(GuestType, int?) SelectGuestType();
        Guest UpdateGuestDetails(Guest currentGuestData);
        string ValidateNameNewGuest(string prompt, string errorMessage);
        string ValidatePhoneNumberNewGuest();
        string ValidateEmailAddressNewGuest();
        string ValidateNameExistingGuest(string prompt, string errorMessage, string currentName);
        string ValidatePhoneNumberExistingGuest(string currentPhoneNumber);
        string ValidateEmailAddressExistingGuest(string currentEmail);

    }
}