package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import java.io.Serializable;
import java.util.List;
import java.util.UUID;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.OneToMany;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;

@Entity
@Table(name = "c_multiple_dependents")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class C_MultipleDependent implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(name = "multiple_dep_attr", nullable = false)
    private String multipleDepAttr;

    @NotNull
    @OneToMany(cascade = { CascadeType.ALL }, orphanRemoval = true, fetch = FetchType.LAZY)
    @JoinColumn(name = "c_multiple_dependent_id", nullable = false)
    private List<C_RequiredComposite> c_RequiredComposites;

    public boolean isNew() {
        return this.id == null;
    }
}
