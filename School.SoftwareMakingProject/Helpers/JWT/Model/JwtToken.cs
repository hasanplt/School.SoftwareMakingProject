using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSchool.SoftwareMakingProject.Helpers.JWT.Model
{
    public sealed class JwtToken
    {
        private JwtSecurityToken token;
        internal JwtToken(JwtSecurityToken token)
        {
            this.token = token;
        }

        public DateTime ValidTo => token.ValidTo;
        public string Value => new JwtSecurityTokenHandler().WriteToken(token);
    }
}
