package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.ExplicitKeys;

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
@Table(name = "fk_a_composite_foreign_keys")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class FK_A_CompositeForeignKey implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "id", nullable = false)
    private UUID id;

    @Column(columnDefinition = "uuid", name = "pk_a_compositekey_composite_key_a", nullable = false)
    private UUID pkACompositekeyCompositeKeyA;

    @Column(columnDefinition = "uuid", name = "pk_a_compositekey_composite_key_b", nullable = false)
    private UUID pkACompositekeyCompositeKeyB;

    @NotNull
    @ManyToOne(optional = false, fetch = FetchType.LAZY)
    @JoinColumn(name = "pk_a_composite_key_id", nullable = false)
    private PK_A_CompositeKey pK_A_CompositeKey;

    public boolean isNew() {
        return this.id == null;
    }
}
