package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models;

import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentManage;
import lombok.*;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.Mode;
import java.io.Serializable;
import java.util.UUID;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.validation.constraints.NotNull;

import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.Setter;

@Entity
@Table(name = "roles")
@RequiredArgsConstructor
@Getter
@Setter
@AllArgsConstructor
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