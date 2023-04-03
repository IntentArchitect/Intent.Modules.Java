package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import lombok.AllArgsConstructor;
import lombok.Data;

import lombok.NoArgsConstructor;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.io.Serializable;
import java.util.UUID;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.OneToOne;
import javax.persistence.Table;

@Entity
@Table(name = "a_required_composites")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class A_RequiredComposite implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uniqueidentifier", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(name = "required_comp_attr", nullable = false)
    private String requiredCompAttr;

    @OneToOne(optional = true, cascade = { CascadeType.ALL }, fetch = FetchType.LAZY)
    @JoinColumn(name="a_optional_dependent_id", nullable = true)
    private A_OptionalDependent a_OptionalDependent;

    public boolean isNew() {
        return this.id == null;
    }
}