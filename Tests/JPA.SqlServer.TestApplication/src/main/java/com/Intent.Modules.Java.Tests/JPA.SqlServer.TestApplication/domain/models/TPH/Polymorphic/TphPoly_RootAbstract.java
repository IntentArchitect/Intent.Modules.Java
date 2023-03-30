package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPH.Polymorphic;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.io.Serializable;
import java.util.UUID;

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