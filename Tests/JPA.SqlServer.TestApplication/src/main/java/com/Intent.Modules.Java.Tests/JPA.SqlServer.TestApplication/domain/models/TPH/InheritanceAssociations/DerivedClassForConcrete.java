package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPH.InheritanceAssociations;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.Size;
import lombok.AllArgsConstructor;
import lombok.Data;

@Entity
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class DerivedClassForConcrete extends ConcreteBaseClass {
    private static final long serialVersionUID = 1L;

    @NotNull
    @Size(max = 250)
    @Column(name = "derived_attribute", length = 250, nullable = false)
    private String derivedAttribute;
}