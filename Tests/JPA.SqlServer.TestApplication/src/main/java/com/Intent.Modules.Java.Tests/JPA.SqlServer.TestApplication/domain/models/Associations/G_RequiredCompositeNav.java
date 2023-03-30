package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentManageClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.Mode;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.io.Serializable;
import java.util.List;
import java.util.UUID;

@Entity
@Table(name = "g_required_composite_navs")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class G_RequiredCompositeNav implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uniqueidentifier", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(name = "req_comp_nav_attr", nullable = false)
    private String reqCompNavAttr;

    @NotNull
    @OneToMany(cascade = { CascadeType.ALL }, orphanRemoval = true, mappedBy="g_RequiredCompositeNav", fetch = FetchType.LAZY)
    private List<G_MultipleDependent> g_MultipleDependents;

    public boolean isNew() {
        return this.id == null;
    }
}