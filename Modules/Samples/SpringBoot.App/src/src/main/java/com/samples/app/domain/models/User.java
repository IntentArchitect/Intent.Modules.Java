package com.samples.app.domain.models;

import java.util.List;
import javax.validation.constraints.Size;

import com.samples.app.intent.IntentIgnore;
import com.samples.app.intent.IntentManage;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.Email;
import lombok.Data;
import lombok.EqualsAndHashCode;
import javax.persistence.*;



@EqualsAndHashCode(callSuper = true)
@Entity
@Table(name = "user")
@Data
@IntentManage
public class User extends AbstractEntity {
    private static final long serialVersionUID = 1L;
    
    @IntentIgnore
    public User() {}

    @IntentIgnore
    public User(String firstName, String lastName, String fullName, String phoneNumber, String username, String email, String password) {
        super();
        this.firstName = firstName;
        this.lastName = lastName;
        this.fullName = fullName;
        this.phoneNumber = phoneNumber;
        this.username = username;
        this.email = email;
        this.password = password;
    }
    
    @Size(max = 100)
    @NotNull
    @Column(name = "first_name", length = 100, nullable = false)
    private String firstName;
    
    @Size(max = 100)
    @NotNull
    @Column(name = "last_name", length = 100, nullable = false)
    private String lastName;
    
    @Size(max = 100)
    @NotNull
    @Column(name = "full_name", length = 100, nullable = false)
    private String fullName;
    
    @NotNull
    @Column(name = "phone_number", nullable = false)
    private String phoneNumber;
    
    @Size(max = 50)
    @NotNull
    @Column(name = "username", length = 50, nullable = false)
    private String username;
    
    @Size(max = 254)
    @Email
    @NotNull
    @Column(name = "email", length = 254, nullable = false)
    private String email;
    
    @Size(max = 60)
    @NotNull
    @Column(name = "password", length = 60, nullable = false)
    private String password;

    
    @ManyToMany
    @JoinTable(
            name = "user_role",
            joinColumns = { @JoinColumn(name = "user_id") },
            inverseJoinColumns = { @JoinColumn(name = "role_id") }
    )
    private List<Role> roles;
    
    @OneToOne(optional = true, orphanRemoval = true)
    @JoinColumn(name="brand_ambassador_profile_id", nullable = true)
    private BrandAmbassadorProfile brandAmbassadorProfile;
    
    @OneToMany(orphanRemoval = true)
    private List<LoginSession> loginSessions;
}