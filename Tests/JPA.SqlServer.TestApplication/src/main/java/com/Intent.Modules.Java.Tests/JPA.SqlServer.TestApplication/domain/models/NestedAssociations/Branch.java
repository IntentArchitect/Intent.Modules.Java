package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.NestedAssociations;

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
@Table(name = "branches")
@Data
@AllArgsConstructor
@NoArgsConstructor
@IntentManageClass(privateMethods = Mode.Ignore)
public class Branch implements Serializable {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uniqueidentifier", name = "id", nullable = false)
    private UUID id;

    @NotNull
    @Column(name = "branch_attribute", nullable = false)
    private String branchAttribute;

    @NotNull
    @ManyToOne(optional = false, fetch = FetchType.LAZY)
    @JoinColumn(name = "texture_id", nullable = false)
    private Texture texture;

    @NotNull
    @OneToOne(optional = false, cascade = { CascadeType.ALL }, fetch = FetchType.LAZY)
    @JoinColumn(name="internode_id", nullable = false)
    private Internode internode;

    @NotNull
    @ManyToMany(fetch = FetchType.LAZY)
    @JoinTable(
            name = "branch_inhabitants",
            joinColumns = { @JoinColumn(name = "branch_id") },
            inverseJoinColumns = { @JoinColumn(name = "inhabitant_id") }
    )
    private List<Inhabitant> inhabitants;

    @NotNull
    @OneToMany(cascade = { CascadeType.ALL }, orphanRemoval = true, fetch = FetchType.LAZY)
    @JoinColumn(name = "branch_id", nullable = false)
    private List<Leaf> leaves;

    @NotNull
    @ManyToOne(optional = false, fetch = FetchType.LAZY)
    @JoinColumn(name = "tree_id", nullable = false)
    private Tree tree;

    public boolean isNew() {
        return this.id == null;
    }
}