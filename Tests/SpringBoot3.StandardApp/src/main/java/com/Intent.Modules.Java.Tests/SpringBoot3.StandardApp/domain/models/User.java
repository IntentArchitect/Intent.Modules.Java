package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models;

import lombok.*;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.Mode;
import java.io.Serializable;
import java.util.List;
import java.util.UUID;
import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Email;

import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.Setter;

@Entity
@Table(name = "users")
@RequiredArgsConstructor
@Getter
@Setter
@AllArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class User implements Serializable {
    private static final long serialVersionUID = 1L;

    @NotNull
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @NotNull
    @Column(name = "username", nullable = false)
    private String username;

    @NotNull
    @Email
    @NotNull
    @Column(name = "email", nullable = false)
    private String email;

    @NotNull
    
    @OneToMany(cascade = { CascadeType.ALL }, orphanRemoval = true, fetch = FetchType.LAZY)
    @JoinColumn(name = "user_id", nullable = false)
    private List<Role> roles;

    public boolean isNew() {
        return this.id == null;
    }
}