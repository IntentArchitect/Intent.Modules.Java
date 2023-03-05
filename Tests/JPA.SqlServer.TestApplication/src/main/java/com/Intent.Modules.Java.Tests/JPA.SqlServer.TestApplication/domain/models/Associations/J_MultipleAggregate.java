package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import java.io.Serializable;
import java.util.UUID;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;

@Entity
@Table(name = "j_multiple_aggregates")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class J_MultipleAggregate implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(name = "multiple_aggr_attr", nullable = false)
    private String multipleAggrAttr;

    @NotNull
    @ManyToOne(optional = false, fetch = FetchType.LAZY)
    @JoinColumn(name = "j_required_dependent_id", nullable = false)
    private J_RequiredDependent j_RequiredDependent;

    public boolean isNew() {
        return this.id == null;
    }
}
