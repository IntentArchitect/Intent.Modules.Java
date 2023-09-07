package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.InheritanceAssociations;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.SecondaryTable;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.Size;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.Setter;

@Entity
@SecondaryTable(name = TpcDerivedClassForConcrete.TABLE_NAME)
@Table(name = "TpcDerivedClassForConcrete")
@Getter
@Setter
@AllArgsConstructor
@RequiredArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class TpcDerivedClassForConcrete extends TpcConcreteBaseClass {
    private static final long serialVersionUID = 1L;

    static final String TABLE_NAME = "tpc_derived_class_for_concretes";

    @NotNull
    @Size(max = 250)
    @Column(name = "derived_attribute", length = 250, table = TABLE_NAME, nullable = false)
    private String derivedAttribute;
}