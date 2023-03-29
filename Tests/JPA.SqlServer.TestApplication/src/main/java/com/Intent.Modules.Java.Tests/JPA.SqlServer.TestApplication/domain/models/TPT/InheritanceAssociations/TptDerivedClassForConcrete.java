package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPT.InheritanceAssociations;
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
@SecondaryTable(name = TptDerivedClassForConcrete.TABLE_NAME)
@Table(name = "TptDerivedClassForConcrete")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class TptDerivedClassForConcrete extends TptConcreteBaseClass {
    private static final long serialVersionUID = 1L;

    static final String TABLE_NAME = "tpt_derived_class_for_concretes";

    @NotNull
    @Size(max = 250)
    @Column(name = "derived_attribute", length = 250, table = TABLE_NAME, nullable = false)
    private String derivedAttribute;
}