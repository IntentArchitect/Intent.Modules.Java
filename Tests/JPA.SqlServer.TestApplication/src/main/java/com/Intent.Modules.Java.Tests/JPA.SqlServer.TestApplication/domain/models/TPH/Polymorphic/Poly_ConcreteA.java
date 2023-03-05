package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPH.Polymorphic;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;

@Entity
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class Poly_ConcreteA extends Poly_BaseClassNonAbstract {
    private static final long serialVersionUID = 1L;

    @NotNull
    @Column(name = "concrete_field", nullable = false)
    private String concreteField;
}
