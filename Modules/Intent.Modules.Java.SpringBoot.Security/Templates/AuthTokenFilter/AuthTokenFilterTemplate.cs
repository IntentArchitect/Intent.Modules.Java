using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly:IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileStringInterpolationTemplate",Version= "1.0")]

namespace Intent.Modules.Java.SpringBoot.Security.Templates.AuthTokenFilter
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class AuthTokenFilterTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return $@"
package {Package};

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.web.authentication.WebAuthenticationDetailsSource;
import org.springframework.util.StringUtils;
import org.springframework.web.filter.OncePerRequestFilter;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;

public class {ClassName} extends OncePerRequestFilter {{
    @Autowired
    private UserDetailsService userDetailsService;
    @Autowired
    private {this.GetJwtUtilsName()} jwtUtils;

    @Override
    protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response, FilterChain filterChain)
            throws ServletException, IOException {{
        String jwt = parseJwt(request);
        if (jwt == null || !jwtUtils.validateJwtToken(jwt)) {{
            filterChain.doFilter(request, response);
            return;
        }}

        String username = jwtUtils.getUserNameFromJwtToken(jwt);
        UserDetails userDetails = userDetailsService.loadUserByUsername(username);
        UsernamePasswordAuthenticationToken authentication = new UsernamePasswordAuthenticationToken(
                userDetails, null, userDetails.getAuthorities());
        authentication.setDetails(new WebAuthenticationDetailsSource().buildDetails(request));
        SecurityContextHolder.getContext().setAuthentication(authentication);

        filterChain.doFilter(request, response);
    }}

    private String parseJwt(HttpServletRequest request) {{
        String headerAuth = request.getHeader(""Authorization"");
        if (StringUtils.hasText(headerAuth) && headerAuth.startsWith(""Bearer "")) {{
            return headerAuth.substring(""Bearer "".length());
        }}

        return null;
    }}
}}
";
        }
    }
}