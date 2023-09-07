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
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.Setter;

@Entity
@SecondaryTable(name = TptPoly_ConcreteB.TABLE_NAME)
@Table(name = "TptPoly_ConcreteB")
@Getter
@Setter
@AllArgsConstructor
@RequiredArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class TptPoly_ConcreteB extends TptPoly_BaseClassNonAbstract {
    private static final long serialVersionUID = 1L;

    static final String TABLE_NAME = "tpt_poly_concrete_bs";

    @NotNull
    @Column(name = "concrete_field", table = TABLE_NAME, nullable = false)
    private String concreteField;
}