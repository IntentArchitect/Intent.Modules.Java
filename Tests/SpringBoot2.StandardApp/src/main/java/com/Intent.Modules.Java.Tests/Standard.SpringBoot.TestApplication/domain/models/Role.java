package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.Mode;
import java.io.Serializable;
import java.util.UUID;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.Setter;

@Entity
@Table(name = "roles")
@Getter
@Setter
@AllArgsConstructor
@RequiredArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class Role implements Serializable {
    private static final long serialVersionUID = 1L;

    @NotNull
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(columnDefinition = "uuid", name = "user_id", nullable = false, insertable = false, updatable = false)
    private UUID userId;

    @NotNull
    @NotNull
    @Column(name = "name", nullable = false)
    private String name;

    public boolean isNew() {
        return this.id == null;
    }
}