package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.InheritanceAssociations;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import java.io.Serializable;
import java.util.UUID;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;

@Entity
@Table(name = "fk_base_class_associateds")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class FkBaseClassAssociated implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(name = "associated_field", nullable = false)
    private String associatedField;

    @Column(columnDefinition = "uuid", name = "fk_base_class_composite_key_a", nullable = false)
    private UUID fkBaseClassCompositeKeyA;

    @Column(columnDefinition = "uuid", name = "fk_base_class_composite_key_b", nullable = false)
    private UUID fkBaseClassCompositeKeyB;

    @NotNull
    @ManyToOne(optional = false, fetch = FetchType.LAZY)
    @JoinColumn(name = "fk_base_class_id", nullable = false)
    private FkBaseClass fkBaseClass;

    public boolean isNew() {
        return this.id == null;
    }
}