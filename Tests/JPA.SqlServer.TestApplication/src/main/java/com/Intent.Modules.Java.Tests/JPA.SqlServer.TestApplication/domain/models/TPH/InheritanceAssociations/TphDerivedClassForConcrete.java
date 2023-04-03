package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPH.InheritanceAssociations;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import lombok.AllArgsConstructor;
import lombok.Data;

import lombok.NoArgsConstructor;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.Size;

@Entity
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class TphDerivedClassForConcrete extends TphConcreteBaseClass {
    private static final long serialVersionUID = 1L;

    @NotNull
    @Size(max = 250)
    @Column(name = "derived_attribute", length = 250, nullable = false)
    private String derivedAttribute;
}