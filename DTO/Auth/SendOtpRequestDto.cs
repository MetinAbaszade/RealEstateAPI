using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Auth;
public record SendOtpRequestDto
{
    public required string ContactNumber { get; set; } = default!;
}