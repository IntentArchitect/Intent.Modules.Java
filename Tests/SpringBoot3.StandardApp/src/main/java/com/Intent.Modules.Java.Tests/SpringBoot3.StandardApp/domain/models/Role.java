package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.Mode;
import java.io.Serializable;
import java.util.UUID;
import lombok.AllArgsConstructor;
import lombok.Data;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.validation.constraints.NotNull;

@Entity
@Table(name = "roles")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class Role implements Serializable {
    private static final long serialVersionUID = 1L;

    @NotNull
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @NotNull
    @Column(name = "name", nullable = false)
    private String name;

    @NotNull
    @Column(columnDefinition = "uuid", name = "user_id", nullable = false, insertable = false, updatable = false)
    private UUID userId;

    public boolean isNew() {
        return this.id == null;
    }
}