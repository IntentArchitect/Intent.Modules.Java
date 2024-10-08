package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Indexes;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import java.io.Serializable;
import java.util.UUID;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Index;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.Setter;

@Entity
@Table(name = "stereotype_indices", indexes = { @Index(name = "IX_StereotypeIndex_defaultIndexField", columnList = "default_index_field"),
        @Index(name = "CustomIndexField", columnList = "custom_index_field"),
        @Index(name = "GroupedIndexField", columnList = "grouped_index_field_a,grouped_index_field_b", unique = true),
        @Index(name = "", columnList = "default_index_field"),
        @Index(name = "CustomIndexField", columnList = "custom_index_field"),
        @Index(name = "GroupedIndexField", columnList = "grouped_index_field_a,grouped_index_field_b", unique = true) })
@Getter
@Setter
@AllArgsConstructor
@RequiredArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class StereotypeIndex implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uniqueidentifier", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(columnDefinition = "uniqueidentifier", name = "default_index_field", nullable = false)
    private UUID defaultIndexField;

    @NotNull
    @Column(columnDefinition = "uniqueidentifier", name = "custom_index_field", nullable = false)
    private UUID customIndexField;

    @NotNull
    @Column(columnDefinition = "uniqueidentifier", name = "grouped_index_field_a", nullable = false)
    private UUID groupedIndexFieldA;

    @NotNull
    @Column(columnDefinition = "uniqueidentifier", name = "grouped_index_field_b", nullable = false)
    private UUID groupedIndexFieldB;

    public boolean isNew() {
        return this.id == null;
    }
}