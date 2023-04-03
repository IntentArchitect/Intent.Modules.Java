package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.InheritanceAssociations;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import lombok.AllArgsConstructor;
import lombok.Data;

import lombok.NoArgsConstructor;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.SecondaryTable;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.Size;

@Entity
@SecondaryTable(name = TpcDerivedClassForConcrete.TABLE_NAME)
@Table(name = "TpcDerivedClassForConcrete")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class TpcDerivedClassForConcrete extends TpcConcreteBaseClass {
    private static final long serialVersionUID = 1L;

    static final String TABLE_NAME = "tpc_derived_class_for_concretes";

    @NotNull
    @Size(max = 250)
    @Column(name = "derived_attribute", length = 250, table = TABLE_NAME, nullable = false)
    private String derivedAttribute;
}