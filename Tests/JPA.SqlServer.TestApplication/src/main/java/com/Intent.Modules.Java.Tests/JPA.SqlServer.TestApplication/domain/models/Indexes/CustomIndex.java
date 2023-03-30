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

@Entity
@Table(name = "custom_indices", indexes = { @Index(name = "IX_CustomIndices_IndexField", columnList = "index_field") })
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class CustomIndex implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uniqueidentifier", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(columnDefinition = "uniqueidentifier", name = "index_field", nullable = false)
    private UUID indexField;

    public boolean isNew() {
        return this.id == null;
    }
}