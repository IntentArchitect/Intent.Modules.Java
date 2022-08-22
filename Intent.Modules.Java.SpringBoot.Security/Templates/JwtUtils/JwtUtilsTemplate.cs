using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly:IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileStringInterpolationTemplate",Version= "1.0")]

namespace Intent.Modules.Java.SpringBoot.Security.Templates.JwtUtils
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class JwtUtilsTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return $@"
package {Package};

import io.jsonwebtoken.JwtParser;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.security.Keys;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Component;

import java.security.Key;
import java.util.Date;

@Component
public class {ClassName} {{
    private final Key key;
    private final JwtParser jwtParser;

    public JwtUtils(@Value(""${{security.jwt.secret}}"") String jwtSecret) {{
        key = Keys.hmacShaKeyFor(jwtSecret.getBytes());
        jwtParser = Jwts.parserBuilder().setSigningKey(jwtSecret.getBytes()).build();
    }}

    public String generateJwtToken(UserDetails userDetails) {{
        return Jwts.builder()
                .setSubject((userDetails.getUsername()))
                .setIssuedAt(new Date())
                .setExpiration(new Date((new Date()).getTime() + 30 * 60 * 1000))
                .signWith(key)
                .compact();
    }}

    public boolean validateJwtToken(String token) {{
        jwtParser.parseClaimsJws(token);
        return true;
    }}

    public String getUserNameFromJwtToken(String token) {{
        return jwtParser.parseClaimsJws(token).getBody().getSubject();
    }}
}}
";
        }
    }
}