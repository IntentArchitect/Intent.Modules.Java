package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.Mode;
import java.io.Serializable;
import java.util.List;
import java.util.UUID;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.OneToMany;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;

@Entity
@Table(name = "users")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class User implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(name = "username", nullable = false)
    private String username;

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
