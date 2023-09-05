package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.security;

import io.jsonwebtoken.JwtParser;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.security.Keys;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Component;

import java.security.Key;
import java.util.Date;

@Component
public class JwtUtils {
    private final Key key;
    private final JwtParser jwtParser;

    public JwtUtils(@Value("${security.jwt.secret}") String jwtSecret) {
        key = Keys.hmacShaKeyFor(jwtSecret.getBytes());
        jwtParser = Jwts.parserBuilder().setSigningKey(jwtSecret.getBytes()).build();
    }

    public String generateJwtToken(UserDetails userDetails) {
        return Jwts.builder()
                .setSubject((userDetails.getUsername()))
                .setIssuedAt(new Date())
                .setExpiration(new Date((new Date()).getTime() + 30 * 60 * 1000))
                .signWith(key)
                .compact();
    }

    public boolean validateJwtToken(String token) {
        jwtParser.parseClaimsJws(token);
        return true;
    }

    public String getUserNameFromJwtToken(String token) {
        return jwtParser.parseClaimsJws(token).getBody().getSubject();
    }
}
