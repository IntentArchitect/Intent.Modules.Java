package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Indexes;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import lombok.AllArgsConstructor;
import lombok.Data;

import lombok.NoArgsConstructor;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.io.Serializable;
import java.util.UUID;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Index;
import javax.persistence.Table;

@Entity
@Table(name = "complex_default_indices", indexes = { @Index(name = "IX_ComplexDefaultIndices_FieldA_FieldB", columnList = "field_a,field_b") })
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class ComplexDefaultIndex implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uniqueidentifier", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(columnDefinition = "uniqueidentifier", name = "field_a", nullable = false)
    private UUID fieldA;

    @NotNull
    @Column(columnDefinition = "uniqueidentifier", name = "field_b", nullable = false)
    private UUID fieldB;

    public boolean isNew() {
        return this.id == null;
    }
}