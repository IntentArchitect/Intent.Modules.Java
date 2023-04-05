package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPT.Polymorphic;

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
@SecondaryTable(name = TptPoly_ConcreteA.TABLE_NAME)
@Table(name = "TptPoly_ConcreteA")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class TptPoly_ConcreteA extends TptPoly_BaseClassNonAbstract {
    private static final long serialVersionUID = 1L;

    static final String TABLE_NAME = "tpt_poly_concrete_as";

    @NotNull
    @Column(name = "concrete_field", table = TABLE_NAME, nullable = false)
    private String concreteField;
}
