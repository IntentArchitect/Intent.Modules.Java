package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.ExplicitKeys;

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
import javax.persistence.IdClass;
import javax.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.Data;

@Entity
@Table(name = "pk_a_composite_keys")
@IdClass(PK_A_CompositeKeyId.class)
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class PK_A_CompositeKey implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "composite_key_a", nullable = false)
    private UUID compositeKeyA;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "composite_key_b", nullable = false)
    private UUID compositeKeyB;

    public boolean isNew() {
        return this.compositeKeyA == null &&
               this.compositeKeyB == null;
    }
}
