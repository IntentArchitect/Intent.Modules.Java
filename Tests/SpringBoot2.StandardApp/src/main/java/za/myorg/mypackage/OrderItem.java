package za.myorg.mypackage;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.util.UUID;

@Entity
@Table(name = "orderItems")
@Data
@AllArgsConstructor
@NoArgsConstructor
public class OrderItem {
    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(columnDefinition = "uuid", name = "id", nullable = false)
    private UUID id;

    @Column(columnDefinition = "uuid", name = "order_id", nullable = false, insertable = false, updatable = false)
    private UUID orderId;

    @NotNull
    @Column(name = "description", nullable = false)
    private String description;
}
