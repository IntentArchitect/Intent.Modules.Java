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
@SecondaryTable(name = Poly_ConcreteB.TABLE_NAME)
@Table(name = "TptPoly_ConcreteB")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class Poly_ConcreteB extends Poly_BaseClassNonAbstract {
    private static final long serialVersionUID = 1L;

    static final String TABLE_NAME = "tpt_poly_concrete_bs";

    @NotNull
    @Column(name = "concrete_field", table = TABLE_NAME, nullable = false)
    private String concreteField;
}