package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.Polymorphic;

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

@Entity
@SecondaryTable(name = TpcPoly_ConcreteB.TABLE_NAME)
@Table(name = "TpcPoly_ConcreteB")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class TpcPoly_ConcreteB extends TpcPoly_BaseClassNonAbstract {
    private static final long serialVersionUID = 1L;

    static final String TABLE_NAME = "tpc_poly_concrete_bs";

    @NotNull
    @Column(name = "concrete_field", table = TABLE_NAME, nullable = false)
    private String concreteField;
}