package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations;

import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import java.io.Serializable;
import java.util.List;
import java.util.UUID;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.OneToMany;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;

@Entity
@Table(name = "h_optional_aggregate_navs")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class H_OptionalAggregateNav implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(name = "optional_aggr_nav_attr", nullable = false)
    private String optionalAggrNavAttr;

    @NotNull
    @OneToMany(mappedBy="h_OptionalAggregateNav", fetch = FetchType.LAZY)
    private List<H_MultipleDependent> h_MultipleDependents;

    public boolean isNew() {
        return this.id == null;
    }
}
