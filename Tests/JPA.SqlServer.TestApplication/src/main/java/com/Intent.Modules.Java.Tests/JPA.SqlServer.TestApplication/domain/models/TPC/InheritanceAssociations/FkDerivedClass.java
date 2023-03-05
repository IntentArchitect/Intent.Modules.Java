package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.InheritanceAssociations;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.SecondaryTable;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;

@Entity
@SecondaryTable(name = FkDerivedClass.TABLE_NAME)
@Table(name = "TpcFkDerivedClass")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class FkDerivedClass extends FkBaseClass {
    private static final long serialVersionUID = 1L;

    static final String TABLE_NAME = "tpc_fk_derived_classes";

    @NotNull
    @Column(name = "derived_field", table = TABLE_NAME, nullable = false)
    private String derivedField;
}
