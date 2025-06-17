using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Cloud_Mall.Application.Stores.Command.CreateStore
{
    public class CreateStoreCommandValidator : AbstractValidator<CreateStoreCommand>
    {
        public CreateStoreCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Store name is required.")
                .MaximumLength(100).WithMessage("Store name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.StoreCategoryID)
                .GreaterThan(0).WithMessage("A valid store category must be selected.");

            RuleFor(x => x.LogoFile)
                .NotNull().WithMessage("A logo file is required.")
                //.Must(HaveValidFileSize).WithMessage("Logo file size must be less than 2MB.")
                .Must(HaveValidFileType).WithMessage("Invalid file type. Only JPG and PNG are allowed.");
        }

        private bool HaveValidFileSize(IFormFile file)
        {
            // Check if file is null (already handled by NotNull validator, but good for safety)
            if (file is null) return true;

            // File size validation (e.g., less than 2MB)
            return file.Length < 2 * 1024 * 1024; // 2 MB
        }

        private bool HaveValidFileType(IFormFile file)
        {
            // Check if file is null
            if (file is null) return true;

            // File type validation
            var allowedTypes = new[] { "image/jpeg", "image/png" };
            return allowedTypes.Contains(file.ContentType);
        }
    }
}