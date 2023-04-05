package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPH.Polymorphic;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import java.io.Serializable;
import java.util.UUID;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.MappedSuperclass;
import javax.persistence.OneToOne;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

@MappedSuperclass
@Table(name = "TphPoly_RootAbstract")
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public abstract class TphPoly_RootAbstract implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uniqueidentifier", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(name = "abstract_field", nullable = false)
    private String abstractField;

    @ManyToOne(optional = true, fetch = FetchType.LAZY)
    @JoinColumn(name = "poly_root_abstract_aggr_id", nullable = true)
    private TphPoly_RootAbstract_Aggr poly_RootAbstract_Aggr;

    @OneToOne(optional = true, cascade = { CascadeType.ALL }, fetch = FetchType.LAZY)
    @JoinColumn(name="poly_root_abstract_comp_id", nullable = true)
    private TphPoly_RootAbstract_Comp poly_RootAbstract_Comp;

    public boolean isNew() {
        return this.id == null;
    }
}
